import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ReservaDto, ReservaResponseDto, DisponibilidadResponseDto } from '../models/reserva.dto';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReservasService {
  private readonly apiUrl = `${environment.apiUrl}/reservas`;

  constructor(private http: HttpClient) {}

  crearReserva(reserva: ReservaDto): Observable<ReservaResponseDto> {
    return this.http.post<ReservaResponseDto>(this.apiUrl, reserva);
  }

  getReservasUsuario(): Observable<ReservaResponseDto[]> {
    return this.http.get<ReservaResponseDto[]>(`${this.apiUrl}/mis-reservas`);
  }

  getDisponibilidad(canchaId: number, fecha: string): Observable<DisponibilidadResponseDto> {
    return this.http.get<DisponibilidadResponseDto>(`${this.apiUrl}/disponibilidad`, {
      params: { canchaId: canchaId.toString(), fecha }
    });
  }

  cancelarReserva(reservaId: number): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${reservaId}/cancelar`, {});
  }

  simularPago(datosPago: any): Observable<{ aprobado: boolean; transaccionId: string }> {
    return new Observable(observer => {
      setTimeout(() => {
        const aprobado = Math.random() > 0.1;
        observer.next({
          aprobado,
          transaccionId: `TXN_${Date.now()}_${Math.random().toString(36).substring(7)}`
        });
        observer.complete();
      }, 2000);
    });
  }
}
