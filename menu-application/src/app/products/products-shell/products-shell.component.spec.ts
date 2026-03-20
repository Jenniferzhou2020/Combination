import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductsShellComponent } from './products-shell.component';

describe('ProductsShellComponent', () => {
  let component: ProductsShellComponent;
  let fixture: ComponentFixture<ProductsShellComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductsShellComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProductsShellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
