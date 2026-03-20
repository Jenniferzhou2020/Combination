import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';


@Component({
  selector: 'app-products-shell',
  standalone: true,
  imports: [RouterOutlet, MatToolbarModule],
  templateUrl: './products-shell.component.html',
  styleUrl: './products-shell.component.css'
})
export class ProductsShellComponent {

}
