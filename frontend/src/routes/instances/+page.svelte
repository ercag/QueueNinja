<script>
	import { onMount } from 'svelte';

	/**
	 * @type {any[]}
	 */
	let instances = [];
	const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

	async function fetchInstances() {
		const res = await fetch(`${API_BASE_URL}/api/instances`);
		instances = await res.json();
	}

	onMount(fetchInstances);
</script>

<h1 class="mb-4 text-2xl font-bold">Monitored Hangfire Instances</h1>

<div class="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3">
	{#each instances as instance}
		<div class="rounded-lg bg-white p-4 shadow">
			<h2 class="text-lg font-semibold">{instance.name}</h2>
			<p class="text-sm text-gray-600">ID: {instance.id}</p>
			<p class="text-sm text-gray-600">
				Last Checked: {new Date(instance.lastChecked).toLocaleString()}
			</p>
			<a
				href={`/jobs/${instance.id}`}
				class="mt-3 block rounded bg-blue-500 py-2 text-center text-white"
			>
				View Jobs
			</a>
		</div>
	{/each}
</div>
