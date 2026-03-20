import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideHttpClient,  withInterceptors } from '@angular/common/http';
import { appConfig } from './app/app.config';
import { authInterceptor } from './app/auth.interceptor';
import { routes } from './app/app.routes'
import { provideRouter } from '@angular/router';
//import { HTTP_INTERCEPTORS } from '@angular/common/http';

bootstrapApplication(AppComponent, {
  ...appConfig,
  providers: [
    ...appConfig.providers,
   // provideHttpClient(withInterceptorsFromDi()),
    provideRouter(routes),
    //{ provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
    provideHttpClient(withInterceptors([authInterceptor])),
  ]
}).catch(err => console.error(err));
