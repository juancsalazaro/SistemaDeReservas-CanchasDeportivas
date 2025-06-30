import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CanchasService } from '../../services/canchas.service';
import { AuthService } from '../../core/auth.service';
import { CanchaResponseDto, CanchaFilters } from '../../models/cancha.dto';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  canchas: CanchaResponseDto[] = [];
  tiposDeporte: string[] = [];
  isLoading = true;
  searchLocation = '';
  selectedSport = '';
  maxPrice: number | null = null;

  filters: CanchaFilters = {};

  constructor(
    private canchasService: CanchasService,
    public authService: AuthService, // Hacer público para usar en template
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  private loadData(): void {
    this.isLoading = true;

    Promise.all([
      this.canchasService.getCanchas().toPromise(),
      this.canchasService.getTiposDeporte().toPromise()
    ]).then(([canchas, tipos]) => {
      this.canchas = canchas || [];
      this.tiposDeporte = tipos || [];
      this.isLoading = false;
    }).catch(error => {
      console.error('Error cargando datos:', error);
      this.isLoading = false;
    });
  }

  onSearch(): void {
    this.filters = {
      tipoDeporte: this.selectedSport || undefined,
      disponible: true,
      precioMaximo: this.maxPrice || undefined
    };

    this.isLoading = true;
    this.canchasService.getCanchas(this.filters).subscribe({
      next: (canchas) => {
        if (this.searchLocation.trim()) {
          this.canchas = canchas.filter(cancha =>
            cancha.ubicacion.toLowerCase().includes(this.searchLocation.toLowerCase())
          );
        } else {
          this.canchas = canchas;
        }
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error en búsqueda:', error);
        this.isLoading = false;
      }
    });
  }

  onFilterBySport(sport: string): void {
    this.selectedSport = sport;
    this.onSearch();
  }

  clearFilters(): void {
    this.searchLocation = '';
    this.selectedSport = '';
    this.maxPrice = null;
    this.filters = {};
    this.loadData();
  }

  viewCancha(id: number): void {
    this.router.navigate(['/cancha', id]);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  // Métodos para manejo de roles y navegación
  navigateToReservas(): void {
    if (this.authService.isCliente()) {
      this.router.navigate(['/mis-reservas']);
    } else {
      alert('Solo los clientes pueden ver sus reservas');
    }
  }

  navigateToCrearCancha(): void {
    if (this.authService.canCreateCanchas()) {
      this.router.navigate(['/crear-cancha']);
    } else {
      alert('No tienes permisos para crear canchas');
    }
  }

  // Métodos para obtener información del usuario
  getUsername(): string {
    return this.authService.getUsername() || 'Usuario';
  }

  getUserRole(): string {
    const role = this.authService.getUserRole();
    return role || 'Sin rol';
  }

  getRoleClass(): string {
    const role = this.authService.getUserRole();
    switch(role) {
      case 'Administrador': return 'role-admin';
      case 'Cliente': return 'role-cliente';
      case 'Empleado': return 'role-empleado';
      default: return 'role-default';
    }
  }

  getRoleIcon(): string {
    const role = this.authService.getUserRole();
    switch(role) {
      case 'Administrador': return 'fas fa-crown';
      case 'Cliente': return 'fas fa-user';
      case 'Empleado': return 'fas fa-user-tie';
      default: return 'fas fa-question';
    }
  }

  // Métodos existentes
  getStarArray(rating: number): number[] {
    return Array(5).fill(0).map((_, i) => i < Math.floor(rating) ? 1 : 0);
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0
    }).format(price);
  }

  getImageUrl(imagenPrincipal: string | null | undefined): string {
    if (imagenPrincipal) {
      return imagenPrincipal;
    }
    return 'assets/default-cancha.jpg';
  }

  parseAmenidades(amenidades: string | null | undefined): string[] {
    if (!amenidades) return [];
    try {
      const parsed = JSON.parse(amenidades);
      return Array.isArray(parsed) ? parsed : [];
    } catch {
      return amenidades.split(',').map(a => a.trim()).filter(a => a.length > 0);
    }
  }

  getSportIcon(tipoDeporte: string): string {
    const iconMap: { [key: string]: string } = {
      'Futbol': 'fas fa-futbol',
      'Fútbol': 'fas fa-futbol',
      'Tenis': 'fas fa-table-tennis',
      'Básquet': 'fas fa-basketball-ball',
      'Basquet': 'fas fa-basketball-ball',
      'Básquetbol': 'fas fa-basketball-ball',
      'Basquetbol': 'fas fa-basketball-ball',
      'Voleibol': 'fas fa-volleyball-ball',
      'Vóleibol': 'fas fa-volleyball-ball',
      'Paddle': 'fas fa-table-tennis',
      'Pádel': 'fas fa-table-tennis',
      'Squash': 'fas fa-table-tennis',
      'Hockey': 'fas fa-hockey-puck',
      'Rugby': 'fas fa-football-ball',
      'Baseball': 'fas fa-baseball-ball',
      'Béisbol': 'fas fa-baseball-ball',
      'Ping Pong': 'fas fa-table-tennis',
      'Natación': 'fas fa-swimmer',
      'Natacion': 'fas fa-swimmer',
      'Gimnasio': 'fas fa-dumbbell',
      'CrossFit': 'fas fa-dumbbell',
      'Yoga': 'fas fa-leaf',
      'Pilates': 'fas fa-leaf',
      'Atletismo': 'fas fa-running',
      'Ciclismo': 'fas fa-bicycle',
      'default': 'fas fa-running'
    };

    return iconMap[tipoDeporte] || iconMap['default'];
  }
}
