export enum LogLevel {
  DEBUG = 'DEBUG',
  INFO = 'INFO',
  WARN = 'WARN',
  ERROR = 'ERROR',
  FATAL = 'FATAL'
}

export interface LogEntry {
  timestamp: Date;
  level: LogLevel;
  message: string;
  data?: any;
  stack?: string;
  url?: string;
  userAgent?: string;
}

export interface LogConfig {
  enableConsole: boolean;
  enableStorage: boolean;
  maxLogEntries: number;
  logLevel: LogLevel;
}
