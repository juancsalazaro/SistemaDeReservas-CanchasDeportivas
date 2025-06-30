export interface UserDto {
  username: string;
  password: string;
  rol?: string;
}

export interface LoginResponse {
  token: string;
  user?: {
    id: number;
    username: string;
    rol: string;
  };
}

export enum UserRole {
  Cliente = 'Cliente',
  Administrador = 'Administrador',
  Empleado = 'Empleado'
}
