import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CanchaDto, CanchaResponseDto, CanchaFilters } from '../models/cancha.dto';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CanchasService {
  private readonly apiUrl = `${environment.apiUrl}/canchas`;

  constructor(private http: HttpClient) {}

  getCanchas(filters?: CanchaFilters): Observable<CanchaResponseDto[]> {
    let params = new HttpParams();

    if (filters?.tipoDeporte) {
      params = params.set('tipoDeporte', filters.tipoDeporte);
    }
    if (filters?.disponible !== undefined) {
      params = params.set('disponible', filters.disponible.toString());
    }
    if (filters?.precioMaximo) {
      params = params.set('precioMaximo', filters.precioMaximo.toString());
    }

    return this.http.get<CanchaResponseDto[]>(this.apiUrl, { params });
  }

  getCancha(id: number): Observable<CanchaResponseDto> {
    return this.http.get<CanchaResponseDto>(`${this.apiUrl}/${id}`);
  }

  createCancha(cancha: CanchaDto): Observable<CanchaResponseDto> {
    return this.http.post<CanchaResponseDto>(this.apiUrl, cancha);
  }

  getTiposDeporte(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/tipos-deporte`);
  }
}
