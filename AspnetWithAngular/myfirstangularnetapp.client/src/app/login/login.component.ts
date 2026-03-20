import { Component,  inject } from '@angular/core';
import { NgIf } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthFacade } from '../auth.facade'

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  imports: [NgIf,  RouterLink, ReactiveFormsModule],
  standalone:true
})
export class LoginComponent {
  private auth = inject(AuthFacade);
  private fb = inject(FormBuilder);
  private router = inject(Router);


  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  message = '';
  // expose facade signals for template
  vm = {
    loading: this.auth.loading,
    error: this.auth.error,
    isAuth: this.auth.isAuthenticated,
    msg: this.message
  };

  onSubmit() {

    if (this.form.invalid) {
      return;
    }
    try {
      const res = this.auth.login(this.form.value as any);
    }
    catch (e) {
      console.error(e);
      this.message = this.readError(e);
    }
  }

  private readError(err: any): string {
    if (err?.error?.message) return err.error.message;
    if (typeof err?.message === 'string') return err.message;
    return 'Login failed. Please try again.';
  }
}
