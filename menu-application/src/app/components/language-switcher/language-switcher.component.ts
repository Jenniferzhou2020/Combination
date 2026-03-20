import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { TranslateModule } from '@ngx-translate/core';
import { LanguageService } from '../../services/language.service';

@Component({
  selector: 'app-language-switcher',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatMenuModule,
    MatIconModule,
    TranslateModule
  ],
  template: `
    <button mat-icon-button [matMenuTriggerFor]="languageMenu">
      <mat-icon>language</mat-icon>
    </button>
    <mat-menu #languageMenu="matMenu">
      <button 
        mat-menu-item 
        *ngFor="let lang of languages"
        (click)="changeLanguage(lang)"
        [class.active]="currentLanguage === lang">
        {{ getLanguageName(lang) }}
      </button>
    </mat-menu>
  `,
  styles: [`
    .active {
      background-color: rgba(0, 0, 0, 0.04);
      font-weight: 500;
    }
  `]
})
export class LanguageSwitcherComponent {
  languages: string[];
  currentLanguage: string;

  constructor(private languageService: LanguageService) {
    this.languages = this.languageService.getSupportedLanguages();
    this.currentLanguage = this.languageService.getCurrentLanguage();

    this.languageService.currentLanguage$.subscribe(lang => {
      this.currentLanguage = lang;
    });
  }

  changeLanguage(language: string): void {
    this.languageService.setLanguage(language);
  }

  getLanguageName(code: string): string {
    const names: { [key: string]: string } = {
      'en': 'English',
      'fr': 'Français'
    };
    return names[code] || code;
  }
}
