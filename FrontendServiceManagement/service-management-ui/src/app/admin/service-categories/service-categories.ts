import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ServiceCategoryService } from './service-category.service';

export interface ServiceCategory {
  serviceCategoryId: number;
  name: string;
  description?: string;
}

@Component({
  standalone: true,
  selector: 'admin-service-categories',
  templateUrl: './service-categories.html',
  styleUrls: ['./service-categories.css'],
  imports: [CommonModule, FormsModule]
})
export class AdminServiceCategories implements OnInit {

  categories: ServiceCategory[] = [];
  filteredCategories: ServiceCategory[] = [];

  loading = false;
  error = '';

  searchTerm = '';

  // CREATE
  newName = '';
  newDescription = '';

  // EDIT
  editingId: number | null = null;
  editName = '';
  editDescription = '';

  constructor(private service: ServiceCategoryService) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories() {
    this.loading = true;
    this.error = '';

    this.service.getAll().subscribe({
      next: data => {
        this.categories = data;
        this.filteredCategories = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load categories';
        this.loading = false;
      }
    });
  }

  filterCategories() {
    const q = this.searchTerm.toLowerCase().trim();

    this.filteredCategories = this.categories.filter(c =>
      c.name.toLowerCase().includes(q) ||
      (c.description?.toLowerCase().includes(q))
    );
  }

  create() {
    if (!this.newName.trim()) return;

    this.service.create({
      name: this.newName,
      description: this.newDescription
    }).subscribe({
      next: () => {
        this.newName = '';
        this.newDescription = '';
        this.loadCategories();
      },
      error: () => alert('Create failed')
    });
  }

  startEdit(c: ServiceCategory) {
    this.editingId = c.serviceCategoryId;
    this.editName = c.name;
    this.editDescription = c.description ?? '';
  }

  cancelEdit() {
    this.editingId = null;
  }

  saveEdit(id: number) {
    this.service.update(id, {
      name: this.editName,
      description: this.editDescription
    }).subscribe({
      next: () => {
        this.editingId = null;
        this.loadCategories();
      },
      error: () => alert('Update failed')
    });
  }

  delete(id: number) {
    if (!confirm('Delete this category?')) return;

    this.service.delete(id).subscribe({
      next: () => this.loadCategories(),
      error: () => alert('Delete failed')
    });
  }
}
