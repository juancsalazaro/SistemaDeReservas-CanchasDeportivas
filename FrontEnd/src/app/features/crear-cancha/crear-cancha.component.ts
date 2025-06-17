import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CanchasService } from '../../services/canchas.service';
import { AuthService } from '../../core/auth.service';
import { CanchaDto } from '../../models/cancha.dto';

@Component({
  selector: 'app-crear-cancha',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './crear-cancha.component.html',
  styleUrls: ['./crear-cancha.component.css']
})
export class CrearCanchaComponent implements OnInit {
  canchaForm!: FormGroup;
  isLoading = false;
  successMessage = '';
  errorMessage = '';
  imagePreview: string | null = null;

  tiposDeporte = [
    'Fútbol',
    'Tenis',
    'Básquet',
    'Voleibol',
    'Paddle',
    'Squash',
    'Hockey',
    'Baseball',
    'Natación',
    'Gimnasio'
  ];

  amenidadesDisponibles = [
    'Estacionamiento',
    'Vestuarios',
    'Duchas',
    'Iluminación nocturna',
    'Cancha techada',
    'Aire acondicionado',
    'WiFi gratuito',
    'Cafetería',
    'Equipo incluido',
    'Árbitro disponible',
    'Zona de espectadores',
    'Seguridad 24/7'
  ];

  amenidadesSeleccionadas: string[] = [];

  constructor(
    private fb: FormBuilder,
    private canchasService: CanchasService,
    private authService: AuthService,
    public router: Router
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  private initializeForm(): void {
    this.canchaForm = this.fb.group({
      nombre: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      descripcion: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(500)]],
      tipoDeporte: ['', [Validators.required]],
      ubicacion: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(200)]],
      precioPorHora: ['', [Validators.required, Validators.min(1000), Validators.max(500000)]],
      imagenPrincipal: [''],
      disponible: [true]
    });
  }

  toggleAmenidad(amenidad: string): void {
    const index = this.amenidadesSeleccionadas.indexOf(amenidad);
    if (index === -1) {
      this.amenidadesSeleccionadas.push(amenidad);
    } else {
      this.amenidadesSeleccionadas.splice(index, 1);
    }
  }

  isAmenidadSeleccionada(amenidad: string): boolean {
    return this.amenidadesSeleccionadas.includes(amenidad);
  }

  onImageSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      if (!file.type.startsWith('image/')) {
        this.errorMessage = 'Por favor selecciona un archivo de imagen válido';
        return;
      }

      if (file.size > 5 * 1024 * 1024) {
        this.errorMessage = 'La imagen no puede ser mayor a 5MB';
        return;
      }

      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.imagePreview = e.target.result;
        this.canchaForm.patchValue({ imagenPrincipal: e.target.result });
      };
      reader.readAsDataURL(file);
    }
  }

  removeImage(): void {
    this.imagePreview = null;
    this.canchaForm.patchValue({ imagenPrincipal: '' });
    const fileInput = document.getElementById('imagenPrincipal') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.canchaForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getErrorMessage(fieldName: string): string {
    const field = this.canchaForm.get(fieldName);

    if (field?.hasError('required')) {
      return 'Este campo es requerido';
    }
    if (field?.hasError('minlength')) {
      return `Mínimo ${field.errors?.['minlength']?.requiredLength} caracteres`;
    }
    if (field?.hasError('maxlength')) {
      return `Máximo ${field.errors?.['maxlength']?.requiredLength} caracteres`;
    }
    if (field?.hasError('min')) {
      return `El valor mínimo es ${field.errors?.['min']?.min}`;
    }
    if (field?.hasError('max')) {
      return `El valor máximo es ${field.errors?.['max']?.max}`;
    }

    return '';
  }

  onSubmit(): void {
    if (this.canchaForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';

      const formData = this.canchaForm.value;

      const canchaDto: CanchaDto = {
        nombre: formData.nombre,
        descripcion: formData.descripcion,
        tipoDeporte: formData.tipoDeporte,
        ubicacion: formData.ubicacion,
        precioPorHora: formData.precioPorHora,
        imagenPrincipal: formData.imagenPrincipal || undefined,
        amenidades: JSON.stringify(this.amenidadesSeleccionadas),
        disponible: formData.disponible
      };

      this.canchasService.createCancha(canchaDto).subscribe({
        next: (response) => {
          this.successMessage = '¡Cancha creada exitosamente!';
          this.isLoading = false;

          setTimeout(() => {
            this.router.navigate(['/dashboard']);
          }, 2000);
        },
        error: (error) => {
          console.error('Error creando cancha:', error);
          this.errorMessage = error.error?.message || 'Error al crear la cancha. Intenta nuevamente.';
          this.isLoading = false;
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched(): void {
    Object.keys(this.canchaForm.controls).forEach(key => {
      const control = this.canchaForm.get(key);
      control?.markAsTouched();
    });
  }

  navigateToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0
    }).format(price);
  }

  getSportIcon(tipoDeporte: string): string {
    const iconMap: { [key: string]: string } = {
      'Fútbol': 'fas fa-futbol',
      'Tenis': 'fas fa-table-tennis',
      'Básquet': 'fas fa-basketball-ball',
      'Voleibol': 'fas fa-volleyball-ball',
      'Paddle': 'fas fa-table-tennis',
      'Squash': 'fas fa-table-tennis',
      'Hockey': 'fas fa-hockey-puck',
      'Baseball': 'fas fa-baseball-ball',
      'Natación': 'fas fa-swimmer',
      'Gimnasio': 'fas fa-dumbbell'
    };

    return iconMap[tipoDeporte] || 'fas fa-running';
  }

  clearMessages(): void {
    this.successMessage = '';
    this.errorMessage = '';
  }
}
