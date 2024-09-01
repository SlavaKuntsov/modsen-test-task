import { makeAutoObservable } from 'mobx';
import { unauthorize } from '../api/authApi';
import { IUser } from '../types';

class UserStore {
	user: IUser | null = null;
	isAuth: boolean = false;
	isAuth2: boolean = false;

	constructor() {
		makeAutoObservable(this, {
			setUser: true,
			setAuth: true,
			setAuth2: true,
			logout: true,
		});
	}

	setUser = (user: IUser | null) => {
		this.user = user;
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
}

export const userStore = new UserStore();
