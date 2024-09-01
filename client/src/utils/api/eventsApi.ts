import kyCore from '../core/kyCore';
import { getAccessToken } from '../tokens';
import { IEvent } from '../types';

const accessToken = getAccessToken();

export const getEvents = async (): Promise<IEvent[]> => {
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
	console.log('id: ', id);
	try {
		const res = await kyCore
		.get(`Events/GetEvent/${id}`, {
			headers: {
				Authorization: `Bearer ${accessToken}`,
			},
		})
		.json<IEvent>();
		console.log('res: ', res);

			return res;
	} catch (error) {
		console.error('Failed to fetch users:', error);
		throw error;
	}
};
