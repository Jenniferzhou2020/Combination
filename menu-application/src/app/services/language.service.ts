import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { SUPPORTED_LANGUAGES, FALLBACK_LANGUAGE } from '../config/translation.config';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {
  private readonly LANGUAGE_KEY = 'app_language';
  private currentLanguageSubject: BehaviorSubject<string>;
  public currentLanguage$: Observable<string>;
  private isBrowser: boolean;

  constructor(
    private translate: TranslateService,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
    
    const savedLanguage = this.getSavedLanguage();
    const browserLanguage = this.translate.getBrowserLang() || FALLBACK_LANGUAGE;
    const initialLanguage = savedLanguage || 
      (SUPPORTED_LANGUAGES.includes(browserLanguage) ? browserLanguage : FALLBACK_LANGUAGE);

    this.currentLanguageSubject = new BehaviorSubject<string>(initialLanguage);
    this.currentLanguage$ = this.currentLanguageSubject.asObservable();

    this.translate.addLangs(SUPPORTED_LANGUAGES);
    //this.translate.setFallbackLang(FALLBACK_LANGUAGE);
    this.translate.setFallbackLang(FALLBACK_LANGUAGE);
    this.setLanguage(initialLanguage);
  }

  setLanguage(language: string): void {
    if (!SUPPORTED_LANGUAGES.includes(language)) {
      console.warn(`Language ${language} is not supported. Using fallback language.`);
      language = FALLBACK_LANGUAGE;
    }

    this.translate.use(language);
    this.saveLanguage(language);
    this.currentLanguageSubject.next(language);
  }

  getCurrentLanguage(): string {
    return this.currentLanguageSubject.value;
  }

  getSupportedLanguages(): string[] {
    return SUPPORTED_LANGUAGES;
  }

  getTranslation(key: string, params?: any): Observable<string> {
    return this.translate.get(key, params);
  }

  instant(key: string, params?: any): string {
    return this.translate.instant(key, params);
  }

  private saveLanguage(language: string): void {
    if (this.isBrowser) {
      try {
        localStorage.setItem(this.LANGUAGE_KEY, language);
      } catch (error) {
        console.warn('Failed to save language to localStorage:', error);
      }
    }
  }

  private getSavedLanguage(): string | null {
    if (this.isBrowser) {
      try {
        return localStorage.getItem(this.LANGUAGE_KEY);
      } catch (error) {
        console.warn('Failed to get language from localStorage:', error);
        return null;
      }
    }
    return null;
  }
}
