import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { LogEntry, LogLevel, LogConfig } from '../models/log.models';

@Injectable({
  providedIn: 'root'
})
export class LoggerService {
  private logs: LogEntry[] = [];
  private readonly STORAGE_KEY = 'app_logs';
  private readonly MAX_STORAGE_SIZE = 5000; // Maximum number of logs in storage
  private isBrowser: boolean;

  private config: LogConfig = {
    enableConsole: true,
    enableStorage: true,
    maxLogEntries: 1000,
    logLevel: LogLevel.DEBUG
  };

  constructor(@Inject(PLATFORM_ID) platformId: Object) {
    this.isBrowser = isPlatformBrowser(platformId);
    this.loadLogsFromStorage();
    if (this.isBrowser) {
      this.setupGlobalErrorHandler();
    }
  }

  private setupGlobalErrorHandler(): void {
    // Capture unhandled errors
    window.addEventListener('error', (event) => {
      const errorData: any = {
        message: event.message,
        filename: event.filename,
        lineno: event.lineno,
        colno: event.colno
      };

      // Safely extract error properties without modifying the original
      if (event.error) {
        try {
          errorData.errorName = event.error.name;
          errorData.errorMessage = event.error.message;
          errorData.errorStack = event.error.stack;
        } catch (e) {
          errorData.error = String(event.error);
        }
      }

      this.error('Unhandled Error', errorData);
    });

    // Capture unhandled promise rejections
    window.addEventListener('unhandledrejection', (event) => {
      const reasonData: any = {};

      try {
        if (typeof event.reason === 'object' && event.reason !== null) {
          reasonData.message = event.reason.message || String(event.reason);
          reasonData.name = event.reason.name;
          reasonData.stack = event.reason.stack;
        } else {
          reasonData.value = String(event.reason);
        }
      } catch (e) {
        reasonData.value = 'Unable to serialize rejection reason';
      }

      this.error('Unhandled Promise Rejection', reasonData);
    });
  }

  debug(message: string, data?: any): void {
    this.log(LogLevel.DEBUG, message, data);
  }

  info(message: string, data?: any): void {
    this.log(LogLevel.INFO, message, data);
  }

  warn(message: string, data?: any): void {
    this.log(LogLevel.WARN, message, data);
  }

  error(message: string, data?: any, error?: Error): void {
    this.log(LogLevel.ERROR, message, data, error?.stack);
  }

  fatal(message: string, data?: any, error?: Error): void {
    this.log(LogLevel.FATAL, message, data, error?.stack);
  }

  private log(level: LogLevel, message: string, data?: any, stack?: string): void {
    if (!this.shouldLog(level)) {
      return;
    }

    const logEntry: LogEntry = {
      timestamp: new Date(),
      level,
      message,
      data,
      stack,
      url: this.isBrowser ? window.location.href : 'server',
      userAgent: this.isBrowser ? navigator.userAgent : 'server'
    };

    this.logs.push(logEntry);

    // Trim logs if exceeds max
    if (this.logs.length > this.config.maxLogEntries) {
      this.logs = this.logs.slice(-this.config.maxLogEntries);
    }

    if (this.config.enableConsole) {
      this.logToConsole(logEntry);
    }

    if (this.config.enableStorage) {
      this.saveLogsToStorage();
    }
  }

  private shouldLog(level: LogLevel): boolean {
    const levels = [LogLevel.DEBUG, LogLevel.INFO, LogLevel.WARN, LogLevel.ERROR, LogLevel.FATAL];
    const currentLevelIndex = levels.indexOf(this.config.logLevel);
    const logLevelIndex = levels.indexOf(level);
    return logLevelIndex >= currentLevelIndex;
  }

  private logToConsole(entry: LogEntry): void {
    const timestamp = entry.timestamp.toISOString();
    const prefix = `[${timestamp}] [${entry.level}]`;

    switch (entry.level) {
      case LogLevel.DEBUG:
        console.debug(prefix, entry.message, entry.data || '');
        break;
      case LogLevel.INFO:
        console.info(prefix, entry.message, entry.data || '');
        break;
      case LogLevel.WARN:
        console.warn(prefix, entry.message, entry.data || '');
        break;
      case LogLevel.ERROR:
      case LogLevel.FATAL:
        console.error(prefix, entry.message, entry.data || '');
        if (entry.stack) {
          console.error('Stack trace:', entry.stack);
        }
        break;
    }
  }

  private saveLogsToStorage(): void {
    if (this.isBrowser && this.config.enableStorage) {
      try {
        const logsToStore = this.logs.slice(-this.MAX_STORAGE_SIZE);
        localStorage.setItem(this.STORAGE_KEY, JSON.stringify(logsToStore));
      } catch (error) {
        console.error('Failed to save logs to storage:', error);
      }
    }
  }

  private loadLogsFromStorage(): void {
    if (this.isBrowser) {
      try {
        const storedLogs = localStorage.getItem(this.STORAGE_KEY);
        if (storedLogs) {
          this.logs = JSON.parse(storedLogs);
        }
      } catch (error) {
        console.error('Failed to load logs from storage:', error);
      }
    }
  }

  getLogs(level?: LogLevel): LogEntry[] {
    if (level) {
      return this.logs.filter(log => log.level === level);
    }
    return [...this.logs];
  }

  clearLogs(): void {
    this.logs = [];
    if (this.isBrowser) {
      try {
        localStorage.removeItem(this.STORAGE_KEY);
      } catch (error) {
        console.error('Failed to clear logs from storage:', error);
      }
    }
  }

  exportLogsAsFile(filename?: string): void {
    if (!this.isBrowser) {
      console.warn('File export is not available in server-side rendering');
      return;
    }

    const logContent = this.formatLogsForExport();
    const blob = new Blob([logContent], { type: 'text/plain' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    
    const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
    link.href = url;
    link.download = filename || `app-logs-${timestamp}.log`;
    link.click();
    
    window.URL.revokeObjectURL(url);
  }

  exportLogsAsJson(filename?: string): void {
    if (!this.isBrowser) {
      console.warn('File export is not available in server-side rendering');
      return;
    }

    const logContent = JSON.stringify(this.logs, null, 2);
    const blob = new Blob([logContent], { type: 'application/json' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    
    const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
    link.href = url;
    link.download = filename || `app-logs-${timestamp}.json`;
    link.click();
    
    window.URL.revokeObjectURL(url);
  }

  private formatLogsForExport(): string {
    return this.logs.map(log => {
      let entry = `[${log.timestamp}] [${log.level}] ${log.message}`;
      
      if (log.data) {
        entry += `\nData: ${JSON.stringify(log.data, null, 2)}`;
      }
      
      if (log.stack) {
        entry += `\nStack: ${log.stack}`;
      }
      
      if (log.url) {
        entry += `\nURL: ${log.url}`;
      }
      
      entry += '\n' + '-'.repeat(80) + '\n';
      
      return entry;
    }).join('\n');
  }

  setConfig(config: Partial<LogConfig>): void {
    this.config = { ...this.config, ...config };
  }

  getConfig(): LogConfig {
    return { ...this.config };
  }
}
