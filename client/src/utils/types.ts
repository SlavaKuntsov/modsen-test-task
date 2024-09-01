export interface IUser {
	email: string;
	password: string;
	passwordConfirmation?: string;
	firstName?: string;
	lastName?: string;
	dateOfBirth?: string | undefined | null,
	role?: string;
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
