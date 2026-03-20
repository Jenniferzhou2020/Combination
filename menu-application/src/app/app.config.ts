import { ApplicationConfig } from '@angular/core';
import { provideRouter, withEnabledBlockingInitialNavigation } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient, withFetch, withInterceptors, HttpClient } from '@angular/common/http';
import { authInterceptor } from './interceptors/auth.interceptor';
import { errorInterceptor } from './interceptors/error.interceptor';
import { loggingInterceptor } from './interceptors/logging.interceptor';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { importProvidersFrom } from '@angular/core';
import { TransferState } from '@angular/platform-browser';
import { HttpLoaderFactory } from './config/translation.config';


export const appConfig: ApplicationConfig = {

  providers: [provideRouter(routes,withEnabledBlockingInitialNavigation()),
    provideClientHydration(), 
    provideAnimations(),
  provideHttpClient(withFetch(), withInterceptors([loggingInterceptor, errorInterceptor, authInterceptor])), 
  provideAnimationsAsync(),
  importProvidersFrom(
    TranslateModule.forRoot({
      defaultLanguage: 'en',
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient, TransferState]
      }
    }) 
  )
]
};

