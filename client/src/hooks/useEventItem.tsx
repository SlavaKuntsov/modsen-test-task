import { useQuery } from '@tanstack/react-query';
import { getEvent } from '../utils/api/eventsApi';
import { IEvent } from '../utils/types';

export function useEventItem(id: string | null) {
	const { isInitialLoading, error, data } = useQuery<IEvent | undefined, Error>(
		{
			queryKey: ['event'],
			queryFn: async () => {
				if (!id) return undefined;
				return await getEvent(id);
			},
			staleTime: 1000 * 60 * 60,
			refetchOnWindowFocus: false,
			retry: false,
			enabled: Boolean(id),
		}
	);

	return {
		eventItem: data,
		isLoading: isInitialLoading,
		error: error,
	};
}
