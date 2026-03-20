import { Component, signal, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive} from '@angular/router';
import { RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar'
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
//import * as XLSX from 'xlsx';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { LanguageService } from './services/language.service';
import { LanguageSwitcherComponent } from './components/language-switcher/language-switcher.component';
import { AuthFacade } from './facades/auth.facade';
import { TranslateModule } from '@ngx-translate/core';
import { Observable, map } from 'rxjs';
import { AuthUser } from './models/auth.models';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ RouterOutlet,
  RouterLink,
  RouterLinkActive,
  MatToolbarModule,
  MatMenuModule,
  MatButtonModule,
  MatIconModule,
  FormsModule,
  NgIf,
  CommonModule,
  LanguageSwitcherComponent,
  TranslateModule
 ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'menu-app';
  jsonText = signal<string | null>(null);
  isAuthenticated$: Observable<boolean>;
  currentUser: AuthUser | null = null;

  constructor(
    private languageService: LanguageService,
    private authFacade: AuthFacade
  ) {
    this.isAuthenticated$ = this.authFacade.authStatus$.pipe(
      map(status => status.isAuthenticated)
    );
  }

  ngOnInit(): void {
    // Language service is initialized automatically
    this.authFacade.authStatus$.subscribe(status => {
      this.currentUser = status.user;
    });
  }

  logout(): void {
    this.authFacade.logout().subscribe({
      next: () => {
        console.log('Logged out successfully');
      },
      error: (error) => {
        console.error('Logout error:', error);
      }
    });
  }
/*
onFileChange(evt: Event) 
{
    const input = evt.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    const reader = new FileReader();

    // Use ArrayBuffer for accurate binary parse
    reader.onload = () => {
      const data = new Uint8Array(reader.result as ArrayBuffer);
      const wb = XLSX.read(data, { type: 'array' });

      // Pick the first worksheet
      const firstSheetName = wb.SheetNames[0];
      const ws = wb.Sheets[firstSheetName];

      // Convert to JSON (header row is inferred by default)
      const json = XLSX.utils.sheet_to_json(ws, {
        defval: null,      // keep empty cells as null
        raw: false,        // parse numbers/dates to JS types when possible
        dateNF: 'yyyy-mm-dd'
      });

      this.jsonText.set(JSON.stringify(json, null, 2));
    };

    reader.onerror = (e) => {
      console.error('File read error', e);
      this.jsonText.set('File read error.');
    };

    reader.readAsArrayBuffer(file);
  } */

}

