import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthFacade } from './auth.facade';

export const authGuard: CanActivateFn = () => {
  const facade = inject(AuthFacade);
  const router = inject(Router);

  if (facade.isAuthenticated()) {
    //router.navigate(['']);
    return true;
  }
  // router.navigate(['/login']);
  router.navigate(['login']);
  return false;
};
