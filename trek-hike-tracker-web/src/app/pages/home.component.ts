import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { RouteService, Route } from '../services/route.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <!-- Hero Section -->
    <div class="relative bg-gradient-to-br from-green-600 via-green-500 to-blue-600 text-white py-24">
      <div class="max-w-7xl mx-auto px-4 text-center">
        <h1 class="text-5xl font-bold mb-4 animate-pulse">🏔️ Trek & Hike Tracker</h1>
        <p class="text-xl mb-8 opacity-90">Kobalarni kashf eting, tog'larni taya berip o'tving</p>
        <p class="text-lg mb-8 opacity-80">O'zbekiston va dunyo bo'ylab eng zo'r trekking marshrutlari</p>
        <div class="flex gap-4 justify-center">
          <a routerLink="/routes" class="bg-yellow-400 text-green-900 px-8 py-3 rounded-lg font-bold hover:bg-yellow-300 transition transform hover:scale-105">
            🗺️ Marshrut Eksploratsiyasini Boshla
          </a>
          <a routerLink="/register" class="bg-white text-green-700 px-8 py-3 rounded-lg font-bold hover:bg-gray-100 transition">
            📝 Ro'yxatdan O'tish
          </a>
        </div>
      </div>
    </div>

    <!-- Stats Section -->
    <div class="bg-white py-12">
      <div class="max-w-7xl mx-auto px-4">
        <div class="grid grid-cols-3 gap-8 text-center">
          <div>
            <div class="text-4xl font-bold text-green-600">{{ stats.totalRoutes }}</div>
            <p class="text-gray-600 mt-2">Marshrut</p>
          </div>
          <div>
            <div class="text-4xl font-bold text-green-600">{{ stats.totalUsers }}</div>
            <p class="text-gray-600 mt-2">Aktiv Trekker</p>
          </div>
          <div>
            <div class="text-4xl font-bold text-green-600">{{ stats.totalComments }}</div>
            <p class="text-gray-600 mt-2">Tajribalar</p>
          </div>
        </div>
      </div>
    </div>

    <!-- Featured Routes -->
    <div class="max-w-7xl mx-auto px-4 py-16">
      <h2 class="text-3xl font-bold mb-8 text-center text-green-700">✨ Eng Mashhur Marshrutlar</h2>

      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <div *ngFor="let route of routes" class="bg-white rounded-lg shadow-lg hover:shadow-2xl transition transform hover:-translate-y-2">
          <div class="bg-gradient-to-r from-green-400 to-blue-400 h-40"></div>
          <div class="p-6">
            <h3 class="text-xl font-bold text-green-700 mb-2">{{ route.name }}</h3>
            <p class="text-gray-600 text-sm mb-4 line-clamp-2">{{ route.description }}</p>

            <div class="grid grid-cols-2 gap-3 mb-4 text-sm">
              <div>
                <span class="text-gray-500">Masofa:</span>
                <p class="font-bold">{{ route.distanceKm }} km</p>
              </div>
              <div>
                <span class="text-gray-500">Vaqt:</span>
                <p class="font-bold">{{ route.estimatedHours }}h</p>
              </div>
              <div>
                <span class="text-gray-500">Qiyinligi:</span>
                <p class="font-bold" [ngClass]="getDifficultyColor(route.difficulty)">
                  {{ getDifficultyText(route.difficulty) }}
                </p>
              </div>
              <div>
                <span class="text-gray-500">Reyting:</span>
                <p class="font-bold">⭐ {{ route.averageRating.toFixed(1) }}</p>
              </div>
            </div>

            <div class="flex gap-2 mb-4">
              <span *ngFor="let tag of route.tags.slice(0, 2)" class="bg-green-100 text-green-700 text-xs px-3 py-1 rounded-full">
                #{{ tag }}
              </span>
            </div>

            <a [routerLink]="['/routes', route.id]" class="w-full bg-green-600 text-white py-2 rounded-lg hover:bg-green-700 transition text-center block">
              Ko'rish →
            </a>
          </div>
        </div>
      </div>
    </div>

    <!-- CTA Section -->
    <div class="bg-green-700 text-white py-16">
      <div class="max-w-4xl mx-auto px-4 text-center">
        <h2 class="text-3xl font-bold mb-4">O'zingiz marshrutni qo'shmoqchisiz?</h2>
        <p class="text-lg mb-8 opacity-90">Sevib kutgan tog'ingizni hamjamiyatga bo'lishing</p>
        <a routerLink="/create-route" class="bg-yellow-400 text-green-900 px-8 py-3 rounded-lg font-bold hover:bg-yellow-300 transition">
          ➕ Yangi Marshrut Qo'shish
        </a>
      </div>
    </div>

    <!-- Footer -->
    <footer class="bg-gray-900 text-white py-8">
      <div class="max-w-7xl mx-auto px-4 text-center">
        <p>© 2026 Trek & Hike Tracker. Barcha huquqlar saqlanadi. 🏔️</p>
      </div>
    </footer>
  `,
  styles: []
})
export class HomeComponent implements OnInit {
  routes: Route[] = [];
  stats = {
    totalRoutes: 0,
    totalUsers: 0,
    totalComments: 0
  };

  constructor(private routeService: RouteService) {}

  ngOnInit(): void {
    this.loadRoutes();
  }

  loadRoutes(): void {
    this.routeService.getRoutes(1, 6).subscribe({
      next: (response) => {
        this.routes = response.items;
        this.stats.totalRoutes = response.totalCount;
        this.stats.totalUsers = Math.floor(response.totalCount * 1.5);
        this.stats.totalComments = Math.floor(response.totalCount * 3.2);
      }
    });
  }

  getDifficultyText(difficulty: number): string {
    const texts = ['Oson', "O'rta", 'Qiyin', 'Ekstremal'];
    return texts[difficulty] || 'Noma\'lum';
  }

  getDifficultyColor(difficulty: number): string {
    const colors = [
      'text-green-600',
      'text-yellow-600',
      'text-orange-600',
      'text-red-600'
    ];
    return colors[difficulty] || 'text-gray-600';
  }
}
