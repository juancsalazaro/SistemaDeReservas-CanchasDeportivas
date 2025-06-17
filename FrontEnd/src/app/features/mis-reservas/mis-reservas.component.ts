import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ReservasService } from '../../services/reservas.service';
import { AuthService } from '../../core/auth.service';
import { ReservaResponseDto } from '../../models/reserva.dto';

@Component({
  selector: 'app-mis-reservas',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './mis-reservas.component.html',
  styleUrls: ['./mis-reservas.component.css']
})
export class MisReservasComponent implements OnInit {
  reservas: ReservaResponseDto[] = [];
  reservasFiltradas: ReservaResponseDto[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';

  filtroEstado = 'todas';
  filtroFecha = 'todas';

  showCancelModal = false;
  reservaParaCancelar: ReservaResponseDto | null = null;
  isLoadingCancel = false;

  constructor(
    private reservasService: ReservasService,
    private authService: AuthService,
    public router: Router
  ) {}

  ngOnInit(): void {
    this.loadReservas();
  }

  private loadReservas(): void {
    this.isLoading = true;
    this.reservasService.getReservasUsuario().subscribe({
      next: (reservas) => {
        this.reservas = reservas;
        this.aplicarFiltros();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error cargando reservas:', error);
        this.errorMessage = 'Error al cargar las reservas';
        this.isLoading = false;
      }
    });
  }

  onFiltroEstadoChange(estado: string): void {
    this.filtroEstado = estado;
    this.aplicarFiltros();
  }

  onFiltroFechaChange(fecha: string): void {
    this.filtroFecha = fecha;
    this.aplicarFiltros();
  }

  private aplicarFiltros(): void {
    let filtradas = [...this.reservas];

    if (this.filtroEstado !== 'todas') {
      filtradas = filtradas.filter(r => r.estado.toLowerCase() === this.filtroEstado);
    }

    const hoy = new Date();
    const fechaHoy = hoy.toISOString().split('T')[0];

    switch (this.filtroFecha) {
      case 'proximas':
        filtradas = filtradas.filter(r => {
          const fechaReserva = new Date(r.fechaReserva).toISOString().split('T')[0];
          return fechaReserva >= fechaHoy;
        });
        break;
      case 'pasadas':
        filtradas = filtradas.filter(r => {
          const fechaReserva = new Date(r.fechaReserva).toISOString().split('T')[0];
          return fechaReserva < fechaHoy;
        });
        break;
      case 'hoy':
        filtradas = filtradas.filter(r => {
          const fechaReserva = new Date(r.fechaReserva).toISOString().split('T')[0];
          return fechaReserva === fechaHoy;
        });
        break;
    }

    filtradas.sort((a, b) => {
      const fechaA = new Date(a.fechaReserva).getTime();
      const fechaB = new Date(b.fechaReserva).getTime();
      return fechaB - fechaA;
    });

    this.reservasFiltradas = filtradas;
  }

  viewCancha(canchaId: number): void {
    this.router.navigate(['/cancha', canchaId]);
  }

  openCancelModal(reserva: ReservaResponseDto): void {
    this.reservaParaCancelar = reserva;
    this.showCancelModal = true;
  }

  closeCancelModal(): void {
    this.showCancelModal = false;
    this.reservaParaCancelar = null;
  }

  confirmarCancelacion(): void {
    if (!this.reservaParaCancelar) return;

    this.isLoadingCancel = true;
    this.reservasService.cancelarReserva(this.reservaParaCancelar.id).subscribe({
      next: (response) => {
        this.successMessage = 'Reserva cancelada exitosamente';
        this.closeCancelModal();
        this.loadReservas();
        this.isLoadingCancel = false;

        setTimeout(() => {
          this.successMessage = '';
        }, 3000);
      },
      error: (error) => {
        console.error('Error cancelando reserva:', error);
        this.errorMessage = error.error?.message || 'Error al cancelar la reserva';
        this.isLoadingCancel = false;

        setTimeout(() => {
          this.errorMessage = '';
        }, 3000);
      }
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  navigateToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0
    }).format(price);
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('es-CO', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }

  formatTime(dateTimeString: string): string {
    const date = new Date(dateTimeString);
    return date.toLocaleTimeString('es-CO', {
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  getEstadoClass(estado: string): string {
    switch (estado.toLowerCase()) {
      case 'confirmada':
        return 'estado-confirmada';
      case 'cancelada':
        return 'estado-cancelada';
      case 'completada':
        return 'estado-completada';
      default:
        return 'estado-default';
    }
  }

  getEstadoIcon(estado: string): string {
    switch (estado.toLowerCase()) {
      case 'confirmada':
        return 'fas fa-check-circle';
      case 'cancelada':
        return 'fas fa-times-circle';
      case 'completada':
        return 'fas fa-flag-checkered';
      default:
        return 'fas fa-info-circle';
    }
  }

  canCancelReserva(reserva: ReservaResponseDto): boolean {
    if (reserva.estado.toLowerCase() !== 'confirmada') {
      return false;
    }

    const now = new Date();
    const reservaDateTime = new Date(reserva.horaInicio);
    const hoursUntilReserva = (reservaDateTime.getTime() - now.getTime()) / (1000 * 60 * 60);

    return hoursUntilReserva >= 2;
  }

  getTimeUntilReserva(reserva: ReservaResponseDto): string {
    const now = new Date();
    const reservaDateTime = new Date(reserva.horaInicio);
    const diffMs = reservaDateTime.getTime() - now.getTime();

    if (diffMs < 0) {
      return 'Ya pasó';
    }

    const diffHours = Math.floor(diffMs / (1000 * 60 * 60));
    const diffDays = Math.floor(diffHours / 24);

    if (diffDays > 0) {
      return `En ${diffDays} día${diffDays > 1 ? 's' : ''}`;
    } else if (diffHours > 0) {
      return `En ${diffHours} hora${diffHours > 1 ? 's' : ''}`;
    } else {
      const diffMinutes = Math.floor(diffMs / (1000 * 60));
      return `En ${diffMinutes} minuto${diffMinutes > 1 ? 's' : ''}`;
    }
  }

  isReservaPasada(reserva: ReservaResponseDto): boolean {
    const now = new Date();
    const reservaDateTime = new Date(reserva.horaFin);
    return reservaDateTime < now;
  }

  isReservaHoy(reserva: ReservaResponseDto): boolean {
    const hoy = new Date().toISOString().split('T')[0];
    const fechaReserva = new Date(reserva.fechaReserva).toISOString().split('T')[0];
    return fechaReserva === hoy;
  }

  getReservasByEstado(estado: string): number {
    return this.reservas.filter(r => r.estado.toLowerCase() === estado.toLowerCase()).length;
  }

  getTotalReservas(): number {
    return this.reservas.length;
  }
}
