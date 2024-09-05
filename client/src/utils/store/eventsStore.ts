import { makeAutoObservable } from 'mobx';
import { IEvent } from '../types';

class EventStore {
	events: Array<IEvent> | null = null;
	selectedEvent: string | null = null;
	searchingEvent: string | null = null;

	constructor() {
		makeAutoObservable(this, {
			setEvents: true,
			setSelectEvent: true,
			isEventLoading: true,
			setSearchingEvent: true,
		});
	}

	setEvents = (event: Array<IEvent> | null) => {
		this.events = event;
	};

	setSelectEvent = (id: string | null) => {
		this.selectedEvent = id;
	};

	setSearchingEvent = (id: string | null) => {
		this.searchingEvent = id;
	};

	resetStore = () => {
		this.events = null;
		this.selectedEvent = null;
	};

	get isEventLoading() {
		return this.events !== null;
	}
}

export const eventStore = new EventStore();
