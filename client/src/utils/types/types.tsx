export interface IUser {
	email: string;
	password: string;
}

export interface IAuthResult {
	accessToken: string;
	// refreshToken: string;
}

export interface IEvent {
	id: string;
	name: string;
}

export type StatusType = 'success' | 'error' | 'warning' | 'info';
