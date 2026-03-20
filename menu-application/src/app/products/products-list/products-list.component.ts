import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, ActivatedRoute } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { NgIf } from '@angular/common';


@Component({
  selector: 'app-products-list',
  standalone: true,
  imports: [MatListModule, MatButtonModule, RouterLink, RouterLinkActive,NgIf],
  templateUrl: './products-list.component.html',
  styleUrl: './products-list.component.css'
})
export class ProductsListComponent {
 category = this.route.snapshot.queryParamMap.get('category');
  constructor(private route: ActivatedRoute) {}
}
