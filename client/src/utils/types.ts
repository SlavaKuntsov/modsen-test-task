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

export interface IDelete {
	id?: string | null | undefined;
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

export interface IAdmin {
	id?: string | null | undefined;
	email: string;
	isActiveAdmin: boolean;
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

export type SortType = 'title' | 'date' | 'participantCount'