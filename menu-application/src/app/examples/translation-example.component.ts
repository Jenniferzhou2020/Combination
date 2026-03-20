import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { LanguageService } from '../services/language.service';

/**
 * Example component demonstrating how to use translations
 */
@Component({
  selector: 'app-translation-example',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  template: `
    <div class="example-container">
      <h2>{{ 'common.welcome' | translate }}</h2>
      
      <!-- Using translate pipe -->
      <p>{{ 'auth.login' | translate }}</p>
      <p>{{ 'errors.network_error' | translate }}</p>
      
      <!-- Using translate service in component -->
      <button (click)="showTranslation()">
        {{ 'common.confirm' | translate }}
      </button>
      
      <!-- Current language -->
      <p>Current language: {{ currentLanguage }}</p>
    </div>
  `,
  styles: [`
    .example-container {
      padding: 20px;
    }
  `]
})
export class TranslationExampleComponent {
  currentLanguage: string;

  constructor(private languageService: LanguageService) {
    this.currentLanguage = this.languageService.getCurrentLanguage();
    
    this.languageService.currentLanguage$.subscribe(lang => {
      this.currentLanguage = lang;
    });
  }

  showTranslation(): void {
    // Using instant translation
    const message = this.languageService.instant('common.welcome');
    console.log(message);
    
    // Using observable translation
    this.languageService.getTranslation('auth.login_success').subscribe(text => {
      console.log(text);
    });
  }
}
