import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Category } from 'src/interfaces/Category';
import { AdminService } from 'src/services/admin.service';
import { CategoryService } from 'src/services/category.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css'],
})
export class AdminComponent implements OnInit {
  categories: Category[] = [];
  category: Category | undefined;
  categoryEditMode = false;
  categoryForm = new FormGroup({
    title: new FormControl('', [Validators.required]),
  });

  categoryEditForm = new FormGroup({
    title: new FormControl('', [Validators.required]),
    id: new FormControl('', [Validators.required]),
  });

  constructor(
    private adminService: AdminService,
    private categoryService: CategoryService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.categoryService
      .getCategories()
      .subscribe((result) => (this.categories = result));
  }

  exportUsers() {
    this.adminService
      .exportUsers()
      .subscribe((result) => this.downloadFile(result));
  }

  downloadFile(data: Blob) {
    const blob = new Blob([data], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
    });
    const url = window.URL.createObjectURL(blob);
    window.open(url);
  }

  submit() {}

  deleteCategory(id: string) {
    console.log(id);
    this.categoryService.deleteCategory(id).subscribe(() => this.reloadPage());
  }

  reloadPage() {
    const currentUrl = this.router.url;
    this.router
      .navigateByUrl('/login', { skipLocationChange: true })
      .then(() => {
        this.router.navigate([currentUrl]);
      });
  }

  editCategory(id: string) {
    console.log(id, this.categoryEditForm.get('title')?.value!!);

    const body = {
      id: id,
      title: this.categoryEditForm.get('title')?.value!!,
    };

    this.categoryService.updateCategory(body).subscribe((result) => {
      this.reloadPage();
    });
  }

  addCategory() {
    const body = {
      title: this.categoryForm.get('title')?.value!!
    }


    console.log(body);
    this.categoryService.addCategory(body).subscribe((result) => {
      console.log(result);
      location.reload();
    });
  }
}
