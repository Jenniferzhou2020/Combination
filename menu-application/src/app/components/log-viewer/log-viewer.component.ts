import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { LoggerService } from '../../services/logger.service';
import { LogEntry, LogLevel } from '../../models/log.models';

@Component({
  selector: 'app-log-viewer',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatSelectModule,
    MatFormFieldModule,
    FormsModule
  ],
  template: `
    <div class="log-viewer">
      <div class="log-header">
        <h2>Application Logs</h2>
        <div class="log-actions">
          <mat-form-field>
            <mat-label>Filter by Level</mat-label>
            <mat-select [(ngModel)]="selectedLevel" (selectionChange)="filterLogs()">
              <mat-option [value]="null">All</mat-option>
              <mat-option *ngFor="let level of logLevels" [value]="level">
                {{ level }}
              </mat-option>
            </mat-select>
          </mat-form-field>
          
          <button mat-raised-button color="primary" (click)="exportAsText()">
            <mat-icon>download</mat-icon>
            Export as .log
          </button>
          
          <button mat-raised-button color="primary" (click)="exportAsJson()">
            <mat-icon>download</mat-icon>
            Export as .json
          </button>
          
          <button mat-raised-button color="warn" (click)="clearLogs()">
            <mat-icon>delete</mat-icon>
            Clear Logs
          </button>
        </div>
      </div>

      <div class="log-stats">
        <span>Total Logs: {{ filteredLogs.length }}</span>
        <span>Errors: {{ getLogCount('ERROR') }}</span>
        <span>Warnings: {{ getLogCount('WARN') }}</span>
        <span>Info: {{ getLogCount('INFO') }}</span>
      </div>

      <div class="log-entries">
        <div *ngFor="let log of filteredLogs" 
             class="log-entry" 
             [class.log-error]="log.level === 'ERROR' || log.level === 'FATAL'"
             [class.log-warn]="log.level === 'WARN'"
             [class.log-info]="log.level === 'INFO'"
             [class.log-debug]="log.level === 'DEBUG'">
          <div class="log-header-row">
            <span class="log-timestamp">{{ log.timestamp | date:'yyyy-MM-dd HH:mm:ss.SSS' }}</span>
            <span class="log-level">{{ log.level }}</span>
          </div>
          <div class="log-message">{{ log.message }}</div>
          <div class="log-data" *ngIf="log.data">
            <pre>{{ log.data | json }}</pre>
          </div>
          <div class="log-stack" *ngIf="log.stack">
            <pre>{{ log.stack }}</pre>
          </div>
          <div class="log-url" *ngIf="log.url">
            <small>URL: {{ log.url }}</small>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .log-viewer {
      padding: 20px;
      max-width: 1400px;
      margin: 0 auto;
    }

    .log-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 20px;
    }

    .log-actions {
      display: flex;
      gap: 10px;
      align-items: center;
    }

    .log-stats {
      display: flex;
      gap: 20px;
      margin-bottom: 20px;
      padding: 10px;
      background: #f5f5f5;
      border-radius: 4px;
    }

    .log-stats span {
      font-weight: 500;
    }

    .log-entries {
      max-height: 600px;
      overflow-y: auto;
      border: 1px solid #ddd;
      border-radius: 4px;
    }

    .log-entry {
      padding: 12px;
      border-bottom: 1px solid #eee;
      font-family: monospace;
      font-size: 12px;
    }

    .log-entry:last-child {
      border-bottom: none;
    }

    .log-error {
      background-color: #ffebee;
      border-left: 4px solid #f44336;
    }

    .log-warn {
      background-color: #fff3e0;
      border-left: 4px solid #ff9800;
    }

    .log-info {
      background-color: #e3f2fd;
      border-left: 4px solid #2196f3;
    }

    .log-debug {
      background-color: #f5f5f5;
      border-left: 4px solid #9e9e9e;
    }

    .log-header-row {
      display: flex;
      justify-content: space-between;
      margin-bottom: 8px;
    }

    .log-timestamp {
      color: #666;
    }

    .log-level {
      font-weight: bold;
      padding: 2px 8px;
      border-radius: 3px;
      background: rgba(0, 0, 0, 0.1);
    }

    .log-message {
      margin-bottom: 8px;
      font-weight: 500;
    }

    .log-data pre,
    .log-stack pre {
      margin: 8px 0;
      padding: 8px;
      background: rgba(0, 0, 0, 0.05);
      border-radius: 3px;
      overflow-x: auto;
      white-space: pre-wrap;
      word-wrap: break-word;
    }

    .log-url {
      margin-top: 8px;
      color: #666;
    }
  `]
})
export class LogViewerComponent implements OnInit {
  logs: LogEntry[] = [];
  filteredLogs: LogEntry[] = [];
  selectedLevel: LogLevel | null = null;
  logLevels = Object.values(LogLevel);

  constructor(private logger: LoggerService) {}

  ngOnInit(): void {
    this.loadLogs();
  }

  loadLogs(): void {
    this.logs = this.logger.getLogs();
    this.filterLogs();
  }

  filterLogs(): void {
    if (this.selectedLevel) {
      this.filteredLogs = this.logger.getLogs(this.selectedLevel);
    } else {
      this.filteredLogs = this.logger.getLogs();
    }
  }

  getLogCount(level: string): number {
    return this.logs.filter(log => log.level === level).length;
  }

  exportAsText(): void {
    this.logger.exportLogsAsFile();
  }

  exportAsJson(): void {
    this.logger.exportLogsAsJson();
  }

  clearLogs(): void {
    if (confirm('Are you sure you want to clear all logs?')) {
      this.logger.clearLogs();
      this.loadLogs();
    }
  }
}
