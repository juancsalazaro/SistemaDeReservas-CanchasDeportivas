export interface CanchaDto {
  nombre: string;
  descripcion: string;
  tipoDeporte: string;
  ubicacion: string;
  precioPorHora: number;
  imagenPrincipal?: string;
  imagenesAdicionales?: string;
  amenidades?: string;
  disponible: boolean;
}

export interface CanchaResponseDto {
  id: number;
  nombre: string;
  descripcion: string;
  tipoDeporte: string;
  ubicacion: string;
  precioPorHora: number;
  imagenPrincipal?: string;
  imagenesAdicionales?: string;
  calificacion: number;
  numeroCalificaciones: number;
  disponible: boolean;
  amenidades?: string;
  createdAt: string;
  createdByUsername: string;
}

export interface CanchaFilters {
  tipoDeporte?: string;
  disponible?: boolean;
  precioMaximo?: number;
}
