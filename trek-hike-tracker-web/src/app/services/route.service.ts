import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

export interface Route {
  id: number;
  userId: number;
  userName: string;
  name: string;
  description: string;
  startLatitude: number;
  startLongitude: number;
  endLatitude: number;
  endLongitude: number;
  difficulty: number;
  distanceKm: number;
  elevationGainM: number;
  estimatedHours: number;
  season: number;
  region?: string;
  likesCount: number;
  averageRating: number;
  commentsCount: number;
  tags: string[];
  createdAt: string;
  updatedAt?: string;
}

export interface RoutesResponse {
  items: Route[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class RouteService {
  constructor(private http: HttpClient) {}

  getRoutes(
    page = 1,
    pageSize = 10,
    search?: string,
    difficulty?: number,
    region?: string
  ): Observable<RoutesResponse> {
    let url = `${environment.apiUrl}/routes?page=${page}&pageSize=${pageSize}`;
    if (search) url += `&search=${search}`;
    if (difficulty !== undefined) url += `&difficulty=${difficulty}`;
    if (region) url += `&region=${region}`;
    return this.http.get<RoutesResponse>(url);
  }

  getRoute(id: number): Observable<Route> {
    return this.http.get<Route>(`${environment.apiUrl}/routes/${id}`);
  }

  createRoute(data: any): Observable<Route> {
    return this.http.post<Route>(`${environment.apiUrl}/routes`, data);
  }

  updateRoute(id: number, data: any): Observable<Route> {
    return this.http.put<Route>(`${environment.apiUrl}/routes/${id}`, data);
  }

  deleteRoute(id: number): Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}/routes/${id}`);
  }

  likeRoute(routeId: number): Observable<boolean> {
    return this.http.post<boolean>(`${environment.apiUrl}/routes/${routeId}/social/like`, {});
  }

  unlikeRoute(routeId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${environment.apiUrl}/routes/${routeId}/social/like`);
  }
}
