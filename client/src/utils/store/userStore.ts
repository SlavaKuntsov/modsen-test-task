import { action, computed, makeAutoObservable } from 'mobx';
import { unauthorize } from '../api/authApi';
import { IAdmin, IUser } from '../types';

class UserStore {
	user: IUser | null = null;
	admins: IAdmin[] | null = null;
	isAuth: boolean = false;
	isAuth2: boolean = false;

	constructor() {
		makeAutoObservable(this, {
			setUser: action,
			setAuth: action,
			setAdmins: action,
			setAuth2: action,
			logout: action,
			isLoggedIn: computed,
			isAdminsLoading: computed,
		});
	}

	setUser = (user: IUser | null) => {
		this.user = user;
	};

	setAdmins = (admins: IAdmin[] | null) => {
		this.admins = admins;
	};

	setAuth = (bool: boolean) => {
		this.isAuth = bool;
	};

	setAuth2 = (bool: boolean) => {
		this.isAuth2 = bool;
	};

	logout = async () => {
		await unauthorize();
		this.user = null;
		this.setAuth(false);
		this.setAuth2(false);
		localStorage.removeItem('accessToken');
	};

	get isLoggedIn() {
		return this.user !== null;
	}

	get isAdminsLoading() {
		return this.admins !== null;
	}
}

export const userStore = new UserStore();
