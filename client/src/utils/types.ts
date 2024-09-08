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

export interface IUserUpdate {
	id?: string | null | undefined;
	firstName?: string;
	lastName?: string;
	dateOfBirth?: string | undefined | null;
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
	eventDateTime: string;
	location: string;
	category: string;
	maxParticipants: number;
	participantsCount: number;
	image?: string; 
}

export interface IAuthResult {
	accessToken: string;
	// refreshToken: string;
}

export type StatusType = 'success' | 'error' | 'warning' | 'info';

export interface IRoute {
	name: string;
	path: string;
	role: IUserRole;
}
