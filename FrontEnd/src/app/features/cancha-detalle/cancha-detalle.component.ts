import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CanchasService } from '../../services/canchas.service';
import { ReservasService } from '../../services/reservas.service';
import { CanchaResponseDto } from '../../models/cancha.dto';
import { DisponibilidadResponseDto, ReservaDto, PagoSimuladoDto } from '../../models/reserva.dto';
import { AuthService } from 'src/app/core/auth.service';

@Component({
  selector: 'app-cancha-detalle',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './cancha-detalle.component.html',
  styleUrls: ['./cancha-detalle.component.css']
})
export class CanchaDetalleComponent implements OnInit {
  cancha: CanchaResponseDto | null = null;
  disponibilidad: DisponibilidadResponseDto | null = null;
  reservaForm!: FormGroup;
  pagoForm!: FormGroup;

  isLoading = true;
  isLoadingDisponibilidad = false;
  isLoadingReserva = false;
  showReservaModal = false;
  showPagoModal = false;
  reservaData: ReservaDto | null = null;

  errorMessage = '';
  successMessage = '';

  horariosDisponibles = [
    { value: '06:00', label: '6:00 AM' },
    { value: '07:00', label: '7:00 AM' },
    { value: '08:00', label: '8:00 AM' },
    { value: '09:00', label: '9:00 AM' },
    { value: '10:00', label: '10:00 AM' },
    { value: '11:00', label: '11:00 AM' },
    { value: '12:00', label: '12:00 PM' },
    { value: '13:00', label: '1:00 PM' },
    { value: '14:00', label: '2:00 PM' },
    { value: '15:00', label: '3:00 PM' },
    { value: '16:00', label: '4:00 PM' },
    { value: '17:00', label: '5:00 PM' },
    { value: '18:00', label: '6:00 PM' },
    { value: '19:00', label: '7:00 PM' },
    { value: '20:00', label: '8:00 PM' },
    { value: '21:00', label: '9:00 PM' },
    { value: '22:00', label: '10:00 PM' }
  ];

  constructor(
    private route: ActivatedRoute,
    public authService: AuthService,
    private router: Router,
    private fb: FormBuilder,
    private canchasService: CanchasService,
    private reservasService: ReservasService
  ) {}

  ngOnInit(): void {
    this.initializeForms();
    this.loadCancha();
  }

  private initializeForms(): void {
    this.reservaForm = this.fb.group({
      fechaReserva: ['', [Validators.required]],
      horaInicio: ['', [Validators.required]],
      horaFin: ['', [Validators.required]],
      nombreCliente: ['', [Validators.required, Validators.minLength(2)]],
      emailCliente: ['', [Validators.required, Validators.email]],
      telefonoCliente: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      observaciones: ['']
    });

    this.pagoForm = this.fb.group({
      tipoTarjeta: ['Visa', [Validators.required]],
      numeroTarjeta: ['', [Validators.required, Validators.pattern(/^\d{4}\s\d{4}\s\d{4}\s\d{4}$/)]],
      nombreTarjeta: ['', [Validators.required, Validators.minLength(2)]],
      fechaVencimiento: ['', [Validators.required, Validators.pattern(/^\d{2}\/\d{2}$/)]],
      cvv: ['', [Validators.required, Validators.pattern(/^\d{3,4}$/)]]
    });
  }

  private loadCancha(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.canchasService.getCancha(id).subscribe({
        next: (cancha) => {
          this.cancha = cancha;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error cargando cancha:', error);
          this.errorMessage = 'Error al cargar la información de la cancha';
          this.isLoading = false;
        }
      });
    }
  }

  onFechaChange(): void {
    const fecha = this.reservaForm.get('fechaReserva')?.value;
    if (fecha && this.cancha) {
      this.loadDisponibilidad(fecha);
    }
  }

  private loadDisponibilidad(fecha: string): void {
    if (!this.cancha) return;

    this.isLoadingDisponibilidad = true;
    this.reservasService.getDisponibilidad(this.cancha.id, fecha).subscribe({
      next: (disponibilidad) => {
        this.disponibilidad = disponibilidad;
        this.isLoadingDisponibilidad = false;
      },
      error: (error) => {
        console.error('Error cargando disponibilidad:', error);
        this.isLoadingDisponibilidad = false;
      }
    });
  }

  openReservaModal(): void {
    this.showReservaModal = true;
    const today = new Date().toISOString().split('T')[0];
    this.reservaForm.patchValue({ fechaReserva: today });
    this.onFechaChange();
  }

  closeReservaModal(): void {
    this.showReservaModal = false;
    this.reservaForm.reset();
    this.disponibilidad = null;
  }

  onReservaSubmit(): void {
    if (this.reservaForm.valid && this.cancha) {
      const formData = this.reservaForm.value;

      const horaInicio = new Date(`${formData.fechaReserva}T${formData.horaInicio}:00`);
      const horaFin = new Date(`${formData.fechaReserva}T${formData.horaFin}:00`);
      const horas = (horaFin.getTime() - horaInicio.getTime()) / (1000 * 60 * 60);
      const precioTotal = horas * this.cancha.precioPorHora;

      this.reservaData = {
        canchaId: this.cancha.id,
        fechaReserva: formData.fechaReserva,
        horaInicio: horaInicio.toISOString(),
        horaFin: horaFin.toISOString(),
        nombreCliente: formData.nombreCliente,
        emailCliente: formData.emailCliente,
        telefonoCliente: formData.telefonoCliente,
        observaciones: formData.observaciones,
        datosPago: {} as PagoSimuladoDto
      };

      this.showReservaModal = false;
      this.showPagoModal = true;
    } else {
      this.markFormGroupTouched(this.reservaForm);
    }
  }

  onPagoSubmit(): void {
    if (this.pagoForm.valid && this.reservaData) {
      this.isLoadingReserva = true;
      const pagoData = this.pagoForm.value;

      this.reservasService.simularPago(pagoData).subscribe({
        next: (resultadoPago) => {
          if (resultadoPago.aprobado) {
            this.reservaData!.datosPago = pagoData;

            this.reservasService.crearReserva(this.reservaData!).subscribe({
              next: (reserva) => {
                this.successMessage = '¡Reserva creada exitosamente!';
                this.showPagoModal = false;
                this.pagoForm.reset();
                this.isLoadingReserva = false;

                setTimeout(() => {
                  this.router.navigate(['/mis-reservas']);
                }, 2000);
              },
              error: (error) => {
                this.errorMessage = 'Error al crear la reserva. Intenta nuevamente.';
                this.isLoadingReserva = false;
              }
            });
          } else {
            this.errorMessage = 'Pago rechazado. Verifica tus datos e intenta nuevamente.';
            this.isLoadingReserva = false;
          }
        },
        error: () => {
          this.errorMessage = 'Error procesando el pago. Intenta nuevamente.';
          this.isLoadingReserva = false;
        }
      });
    } else {
      this.markFormGroupTouched(this.pagoForm);
    }
  }

  closePagoModal(): void {
    this.showPagoModal = false;
    this.pagoForm.reset();
    this.reservaData = null;
  }

  getImageUrl(imagenPrincipal: string | null | undefined): string {
    return imagenPrincipal || 'assets/default-cancha.jpg';
  }

  parseAmenidades(amenidades: string | null | undefined): string[] {
    if (!amenidades) return [];
    try {
      return JSON.parse(amenidades);
    } catch {
      return amenidades.split(',').map(a => a.trim());
    }
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0
    }).format(price);
  }

  calcularPrecioTotal(): number {
    if (!this.cancha || !this.reservaForm.get('horaInicio')?.value || !this.reservaForm.get('horaFin')?.value) {
      return 0;
    }

    const horaInicio = this.reservaForm.get('horaInicio')?.value;
    const horaFin = this.reservaForm.get('horaFin')?.value;

    const inicio = new Date(`2000-01-01T${horaInicio}:00`);
    const fin = new Date(`2000-01-01T${horaFin}:00`);

    const horas = (fin.getTime() - inicio.getTime()) / (1000 * 60 * 60);
    return horas > 0 ? horas * this.cancha.precioPorHora : 0;
  }

  isHorarioDisponible(hora: string): boolean {
    if (!this.disponibilidad) return true;

    const horario = this.disponibilidad.horariosDisponibles.find(h =>
      h.horaInicio === hora + ':00:00'
    );

    return horario ? horario.disponible : true;
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }

  isFieldInvalid(formGroup: FormGroup, fieldName: string): boolean {
    const field = formGroup.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getErrorMessage(formGroup: FormGroup, fieldName: string): string {
    const field = formGroup.get(fieldName);

    if (field?.hasError('required')) {
      return `Este campo es requerido`;
    }
    if (field?.hasError('email')) {
      return 'Email inválido';
    }
    if (field?.hasError('minlength')) {
      return `Mínimo ${field.errors?.['minlength']?.requiredLength} caracteres`;
    }
    if (field?.hasError('pattern')) {
      return 'Formato inválido';
    }

    return '';
  }

get fechaMinima(): string {
  return new Date().toISOString().split('T')[0];
}

formatDate(dateString: string): string {
  if (!dateString) return '';
  const date = new Date(dateString);
  return date.toLocaleDateString('es-CO', {
    weekday: 'long',
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  });
}

formatTime(timeString: string): string {
  if (!timeString) return '';
  const date = new Date(timeString);
  return date.toLocaleTimeString('es-CO', {
    hour: '2-digit',
    minute: '2-digit'
  });
}
}
