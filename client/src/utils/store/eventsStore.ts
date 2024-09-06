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
			removeEventById: true,
			resetStore: true,
		});
	}

	setEvents = (event: Array<IEvent> | null) => {
		if (
			!event ||
			event.length === 0 ||
			!event.some(e => e.id === this.selectedEvent)
		) {
			this.selectedEvent = null;
		}

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

	removeEventById = (id: string) => {
		if (this.events) {
			this.events = this.events.filter(event => event.id !== id);
		}
	};

	get isEventLoading() {
		return this.events !== null;
	}
}

export const eventStore = new EventStore();
