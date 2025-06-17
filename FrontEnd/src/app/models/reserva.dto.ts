export interface ReservaDto {
  canchaId: number;
  fechaReserva: string;
  horaInicio: string;
  horaFin: string;
  nombreCliente: string;
  emailCliente: string;
  telefonoCliente: string;
  observaciones?: string;
  datosPago: PagoSimuladoDto;
}

export interface PagoSimuladoDto {
  tipoTarjeta: string;
  numeroTarjeta: string;
  nombreTarjeta: string;
  fechaVencimiento: string;
  cvv: string;
}

export interface ReservaResponseDto {
  id: number;
  canchaId: number;
  nombreCancha: string;
  fechaReserva: string;
  horaInicio: string;
  horaFin: string;
  precioTotal: number;
  estado: string;
  nombreCliente: string;
  emailCliente: string;
  telefonoCliente: string;
  metodoPago: string;
  observaciones?: string;
  createdAt: string;
  username: string;
}

export interface DisponibilidadResponseDto {
  fecha: string;
  horariosDisponibles: HorarioDisponibleDto[];
  reservasActivas: ReservaActivaDto[];
}

export interface HorarioDisponibleDto {
  horaInicio: string;
  horaFin: string;
  disponible: boolean;
}

export interface ReservaActivaDto {
  id: number;
  horaInicio: string;
  horaFin: string;
  estado: string;
  nombreCliente: string;
}
