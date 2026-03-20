import { HttpClient } from '@angular/common/http';
import { TranslateLoader } from '@ngx-translate/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { makeStateKey, TransferState } from '@angular/platform-browser';

export class CustomTranslateLoader implements TranslateLoader {
  constructor(
    private http: HttpClient,
    private transferState: TransferState,
    private prefix: string = './assets/i18n/',
    private suffix: string = '.json'
  ) {}

  getTranslation(lang: string): Observable<any> {
    const key = makeStateKey<any>(`transfer-translate-${lang}`);
    const data = this.transferState.get(key, null);

    // If we have data from SSR, use it
    if (data) {
      this.transferState.remove(key);
      return of(data);
    }

    // Otherwise, load from HTTP
    const url = `${this.prefix}${lang}${this.suffix}`;
    
    return this.http.get(url).pipe(
      catchError((error) => {
        console.error(`Failed to load translation file: ${url}`, error);
        return of({});
      })
    );
  }
}

export function HttpLoaderFactory(http: HttpClient, transferState: TransferState): TranslateLoader {
  return new CustomTranslateLoader(http, transferState, './assets/i18n/', '.json');
}

export const SUPPORTED_LANGUAGES = ['en', 'fr'];
export const FALLBACK_LANGUAGE = 'en';

