export interface IUser {
	id?: string | null | undefined;
	email: string;
	password: string;
	passwordConfirmation?: string;
	firstName?: string;
	lastName?: string;
	dateOfBirth?: string | undefined | null;
	role?: string;
}

export enum IUserRole {
	Admin = 'Admin',
	User = 'User',
	All = 'All',
}
export enum IEventsFetch {
	AllEvents = 'AllEvents',
	UserEvents = 'UserEvents',
	// All = 'All',
}

// export type IEventsFetch = 'allEvents' | 'UserEvents'

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
	role: IUserRole
}
