import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { HttpError } from '../interceptors/error.interceptor';

export interface ErrorNotification {
  message: string;
  type: 'error' | 'warning' | 'info';
  timestamp: Date;
}

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {
  private errorSubject = new Subject<ErrorNotification>();
  public error$ = this.errorSubject.asObservable();

  handleError(error: HttpError): void {
    const notification: ErrorNotification = {
      message: error.message,
      type: this.getNotificationType(error.status),
      timestamp: new Date()
    };

    this.errorSubject.next(notification);
  }

  private getNotificationType(status: number): 'error' | 'warning' | 'info' {
    if (status >= 500) {
      return 'error';
    } else if (status >= 400) {
      return 'warning';
    }
    return 'info';
  }

  showError(message: string): void {
    this.errorSubject.next({
      message,
      type: 'error',
      timestamp: new Date()
    });
  }

  showWarning(message: string): void {
    this.errorSubject.next({
      message,
      type: 'warning',
      timestamp: new Date()
    });
  }

  showInfo(message: string): void {
    this.errorSubject.next({
      message,
      type: 'info',
      timestamp: new Date()
    });
  }
}
