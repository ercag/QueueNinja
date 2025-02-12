<script>
	import { page } from '$app/stores';
	import { onMount } from 'svelte';

	const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

	/**
	 * @type {any[]}
	 */
	let jobs = [];
	let instanceId = $page.params.id;
	let jobHistory = {};
	/**
	 * @type {null}
	 */
	let selectedJobId = null;

	async function fetchJobs() {
		const res = await fetch(`${API_BASE_URL}/api/jobs/${instanceId}`);
		jobs = await res.json();
	}

	/**
	 * @param {null} jobId
	 */
	async function fetchJobHistory(jobId) {
		selectedJobId = jobId;
		const res = await fetch(`${API_BASE_URL}/api/jobs/${instanceId}/history/${jobId}`);
		jobHistory = await res.json();
	}

	onMount(fetchJobs);
</script>

<h1 class="mb-4 text-2xl font-bold">Jobs for Instance {instanceId}</h1>

<table class="w-full rounded bg-white p-4 shadow">
	<thead>
		<tr class="bg-gray-200">
			<th class="p-3 text-left">Job ID</th>
			<th class="p-3 text-left">Status</th>
			<th class="p-3 text-left">Created At</th>
			<th class="p-3 text-left">Actions</th>
		</tr>
	</thead>
	<tbody>
		{#each jobs as job}
			<tr class="border-b">
				<td class="p-3">{job.id}</td>
				<td class="p-3">
					<span
						class="rounded px-2 py-1 text-sm text-white"
						class:bg-green-500={job.state === 'Succeeded'}
						class:bg-red-500={job.state === 'Failed'}
						class:bg-yellow-500={job.state === 'Processing'}
					>
						{job.state}
					</span>
				</td>
				<td class="p-3">{new Date(job.createdAt).toLocaleString()}</td>
				<td class="p-3">
					<button
						on:click={() => fetchJobHistory(job.id)}
						class="rounded bg-gray-500 px-3 py-1 text-white">View Logs</button
					>
				</td>
			</tr>
		{/each}
	</tbody>
</table>

<!-- Job History Panel -->
{#if selectedJobId}
	<div class="mt-6 rounded bg-white p-4 shadow">
		<h2 class="text-lg font-bold">Job History for ID {selectedJobId}</h2>
		<ul class="mt-2">
			{#each jobHistory as entry}
				<li class="border-b p-2">
					<span class="font-semibold">{entry.state}</span> @ {new Date(
						entry.createdAt
					).toLocaleString()}
					{#if entry.errorMessage}
						<p class="text-red-500">⚠ {entry.errorMessage}</p>
					{/if}
				</li>
			{/each}
		</ul>
	</div>
{/if}
