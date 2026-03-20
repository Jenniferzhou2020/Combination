import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthFacade } from '../facades/auth.facade';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authFacade = inject(AuthFacade);
  const token = authFacade.getToken();

  // Skip auth header for login and refresh endpoints
  if (req.url.includes('/auth/login') || req.url.includes('/auth/refresh')) {
    return next(req);
  }

  // Add token to request if available
  const clonedRequest = token
    ? req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      })
    : req;

  return next(clonedRequest).pipe(
    catchError((error: HttpErrorResponse) => {
      // If 401 Unauthorized, try to refresh token
      if (error.status === 401 && !req.url.includes('/auth/refresh')) {
        return authFacade.refreshToken().pipe(
          switchMap(() => {
            // Retry the request with new token
            const newToken = authFacade.getToken();
            const retryRequest = req.clone({
              setHeaders: {
                Authorization: `Bearer ${newToken}`
              }
            });
            return next(retryRequest);
          }),
          catchError(refreshError => {
            // If refresh fails, logout user
            authFacade.logout();
            return throwError(() => refreshError);
          })
        );
      }
      
      return throwError(() => error);
    })
  );
};

