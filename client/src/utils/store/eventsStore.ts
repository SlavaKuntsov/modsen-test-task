import { makeAutoObservable } from 'mobx';
import { IEvent } from '../types';

class EventStore {
	events: Array<IEvent> | null = null;
	selectedEvent: string | null = null;

	constructor() {
		makeAutoObservable(this, {
			setEvents: true,
			setSelectEvent: true,
		});
	}

	setEvents = (event: Array<IEvent> | null) => {
		this.events = event;
	};

	setSelectEvent = (id: string) => {
		this.selectedEvent = id;
	};

	get isEventLoading() {
		return this.events !== null;
	}
}

export const eventStore = new EventStore();
