import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';


@Component({
  selector: 'app-products-detail',
  standalone: true,
  imports: [ MatButtonModule],
  templateUrl: './products-detail.component.html',
  styleUrl: './products-detail.component.css'
})
export class ProductsDetailComponent {
 id = this.route.snapshot.paramMap.get('id');

  constructor(private route: ActivatedRoute, private router: Router) {}

  backToList() {
    this.router.navigate(['/products', 'list']);
  }
}
