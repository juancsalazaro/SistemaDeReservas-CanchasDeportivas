export type { TareaDto } from '../models/tarea.dto';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import type { TareaDto } from '../models/tarea.dto';

export interface Tarea { 
  id: number;
  titulo: string;
  descripcion: string;
  completada: boolean;
}

@Injectable({ providedIn: 'root' })
export class TareaService {
  private baseUrl = `${environment.apiUrl}/tareas`;
  constructor(private http: HttpClient) {}

  getTareas(): Observable<Tarea[]> {
    return this.http.get<Tarea[]>(this.baseUrl);
  }

  createTarea(tarea: TareaDto): Observable<Tarea> {
    return this.http.post<Tarea>(this.baseUrl, tarea);
  }
}
