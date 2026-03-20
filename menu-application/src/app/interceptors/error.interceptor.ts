import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { inject } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LoggerService } from '../services/logger.service';

export interface HttpError {
  status: number;
  message: string;
  error?: any;
  url?: string;
}

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const translate = inject(TranslateService);
  const logger = inject(LoggerService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      // Create a safe copy of the error object
      let errorCopy: any = null;
      
      try {
        if (error.error && typeof error.error === 'object') {
          errorCopy = {};
          // Copy only serializable properties
          for (const key in error.error) {
            if (error.error.hasOwnProperty(key)) {
              try {
                errorCopy[key] = error.error[key];
              } catch (e) {
                errorCopy[key] = '[Unable to copy property]';
              }
            }
          }
        } else {
          errorCopy = error.error;
        }
      } catch (e) {
        errorCopy = String(error.error);
      }

      const httpError: HttpError = {
        status: error.status,
        message: getErrorMessage(error, translate),
        error: errorCopy,
        url: error.url || req.url
      };

      // Log error using logger service
      logger.error('HTTP Error Intercepted', httpError);

      // Handle specific error codes
      handleErrorByStatus(httpError, logger);

      return throwError(() => httpError);
    })
  );
};

function getErrorMessage(error: HttpErrorResponse, translate: TranslateService): string {
  if (error.error instanceof ErrorEvent) {
    // Client-side or network error
    return translate.instant('errors.network_error');
  }

  // Server-side error
  switch (error.status) {
    case 0:
      return translate.instant('errors.network_error');
    case 400:
      return error.error?.message || translate.instant('errors.bad_request');
    case 401:
      return translate.instant('errors.unauthorized');
    case 403:
      return translate.instant('errors.forbidden');
    case 404:
      return translate.instant('errors.not_found');
    case 408:
      return translate.instant('errors.timeout');
    case 409:
      return error.error?.message || translate.instant('errors.conflict');
    case 422:
      return error.error?.message || translate.instant('errors.validation_error');
    case 429:
      return translate.instant('errors.rate_limit');
    case 500:
      return translate.instant('errors.server_error');
    case 502:
      return translate.instant('errors.bad_gateway');
    case 503:
      return translate.instant('errors.service_unavailable');
    case 504:
      return translate.instant('errors.gateway_timeout');
    default:
      return error.error?.message || translate.instant('errors.unknown_error');
  }
}

function handleErrorByStatus(httpError: HttpError, logger: LoggerService): void {
  // You can add custom handling for specific status codes
  // For example: show notifications, redirect, etc.
  
  switch (httpError.status) {
    case 401:
      logger.warn('Unauthorized access attempt', { url: httpError.url });
      break;
    case 403:
      logger.warn('Access denied', { url: httpError.url });
      break;
    case 500:
    case 502:
    case 503:
    case 504:
      logger.error('Server error occurred', httpError);
      break;
    default:
      break;
  }
}
