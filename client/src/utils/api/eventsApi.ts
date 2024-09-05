import kyCore from '../core/kyCore';
import { getAccessToken } from '../tokens';
import { IEvent } from '../types';

export const getEvents = async (): Promise<IEvent[]> => {
	console.log('getEvents void')
	const accessToken = getAccessToken();
	try {
		return await kyCore
			.get('Events/GetEvents', {
				headers: {
					Authorization: `Bearer ${accessToken}`,
				},
			})
			.json<IEvent[]>();
	} catch (error) {
		console.error('Failed to fetch users:', error);
		throw error;
	}
};

export const getEventsAdmin = async (): Promise<Event> => {
	const accessToken = getAccessToken();
	try {
		return await kyCore
			.get('Events/GetEventsAdmin', {
				headers: {
					Authorization: `Bearer ${accessToken}`,
				},
			})
			.json<Event>();
	} catch (error) {
		console.error('Failed to fetch users:', error);
		throw error;
	}
};

export const getEvent = async (id: string): Promise<IEvent> => {
	const accessToken = getAccessToken();
	try {
		const res = await kyCore
			.get(`Events/GetEvent/${id}`, {
				headers: {
					Authorization: `Bearer ${accessToken}`,
				},
			})
			.json<IEvent>();

		return res;
	} catch (error) {
		console.error('Failed to fetch users:', error);
		throw error;
	}
};
