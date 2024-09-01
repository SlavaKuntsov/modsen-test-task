export interface IUser {
	email: string;
	password: string;
	passwordConfirmation?: string;
	firstName?: string;
	lastName?: string;
	dateOfBirth?: string | undefined | null;
	role?: string;
}

export interface IEvent {
	id: string;
	title: string;
	description: string;
	eventDateTime: string | undefined | null;
	location: string;
	category: string;
	maxParticipants: number;
	imageUrl: string;
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

export interface IRoute {
	name: string;
	path: string;
}
