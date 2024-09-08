import { HTTPError } from 'ky';
import kyCore from '../core/kyCore';
import { getAccessToken } from '../tokens';
import { IEvent } from '../types';

export const getEvents = async (): Promise<IEvent[]> => {
	console.log('getEvents void');
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

export const getEventParticipant = async (
	id: string | null | undefined
): Promise<IEvent[]> => {
	const accessToken = getAccessToken();
	try {
		const res = await kyCore
			.get(`Events/GetEventsByParticipant/${id}`, {
				headers: {
					Authorization: `Bearer ${accessToken}`,
				},
			})
			.json<IEvent[]>();

		return res;
	} catch (error) {
		console.error('Failed to fetch users:', error);
		// throw error;
		return [];
	}
};

interface eventRegistrationProps {
	eventId: string;
	participantId?: string | null | undefined;
}

export const eventRegistration = async ({
	eventId,
	participantId,
}: eventRegistrationProps): Promise<boolean | string> => {
	const accessToken = getAccessToken();
	try {
		const res = await kyCore.post(`Events/RegistrationOnEvent`, {
			headers: {
				Authorization: `Bearer ${accessToken}`,
			},
			json: { eventId, participantId },
		});
		if (res.ok) return true;
		return false;
	} catch (error: unknown) {
		if (error instanceof HTTPError && error.response) {
			const errorMessage = await error.response.text();
			console.error('Failed to registration on event:', errorMessage);
			return errorMessage;
		}

		console.error('Unexpected error:', error);
		return 'An unexpected error occurred';
	}
};

export const eventUnregistration = async ({
	eventId,
	participantId,
}: eventRegistrationProps): Promise<boolean | string> => {
	const accessToken = getAccessToken();
	try {
		const res = await kyCore.post(`Events/UnregistrationOnEvent`, {
			headers: {
				Authorization: `Bearer ${accessToken}`,
			},
			json: { eventId, participantId },
		});
		if (res.ok) return true;
		return false;
	} catch (error: unknown) {
		if (error instanceof HTTPError && error.response) {
			const errorMessage = await error.response.text();
			console.error('Failed to unregistration on event:', errorMessage);
			return errorMessage;
		}

		console.error('Unexpected error:', error);
		return 'An unexpected error occurred';
	}
};

export const createEvent = async (event: IEvent): Promise<string> => {
	const accessToken = getAccessToken();
	try {
		const res = await kyCore
			.post(`Events/CreateEvent`, {
				headers: {
					Authorization: `Bearer ${accessToken}`,
				},
				json: event,
			})
			.json<string>();

		return res;
	} catch (error: unknown) {
		if (error instanceof HTTPError && error.response) {
			const errorMessage = await error.response.text();
			console.error('Failed to create event:', errorMessage);
			return errorMessage;
		}

		console.error('Unexpected error:', error);
		return 'An unexpected error occurred';
	}
};

export const updateEvent = async (event: IEvent): Promise<string | boolean> => {
	const accessToken = getAccessToken();
	try {
		const res = await kyCore
			.put(`Events/Update`, {
				headers: {
					Authorization: `Bearer ${accessToken}`,
				},
				json: event,
			})
			.json<string>();

		return true;
	} catch (error: unknown) {
		if (error instanceof HTTPError && error.response) {
			const errorMessage = await error.response.text();
			console.error('Failed to update event:', errorMessage);
			return errorMessage;
		}

		console.error('Unexpected error:', error);
		return 'An unexpected error occurred';
	}
};
