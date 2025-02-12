<script>
  import { onMount } from "svelte";

  const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

  let jobs = [];
  let instanceId = $page.params.id;
  let jobHistory = {};
  let selectedJobId = null;

  async function fetchJobs() {
    const res = await fetch(`${API_BASE_URL}/api/jobs/${instanceId}`);
    jobs = await res.json();
  }

  async function fetchJobHistory(jobId) {
    selectedJobId = jobId;
    const res = await fetch(`${API_BASE_URL}/api/jobs/${instanceId}/history/${jobId}`);
    jobHistory = await res.json();
  }

  onMount(fetchJobs);
</script>


<h1 class="text-2xl font-bold mb-4">Jobs for Instance {instanceId}</h1>

<table class="w-full bg-white shadow rounded p-4">
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
          <span class="px-2 py-1 rounded text-white text-sm"
            class:bg-green-500={job.state === "Succeeded"}
            class:bg-red-500={job.state === "Failed"}
            class:bg-yellow-500={job.state === "Processing"}
          >
            {job.state}
          </span>
        </td>
        <td class="p-3">{new Date(job.createdAt).toLocaleString()}</td>
        <td class="p-3">
          <button on:click={() => fetchJobHistory(job.id)} class="bg-gray-500 text-white px-3 py-1 rounded">View Logs</button>
        </td>
      </tr>
    {/each}
  </tbody>
</table>

<!-- Job History Panel -->
{#if selectedJobId}
  <div class="mt-6 bg-white shadow rounded p-4">
    <h2 class="text-lg font-bold">Job History for ID {selectedJobId}</h2>
    <ul class="mt-2">
      {#each jobHistory as entry}
        <li class="p-2 border-b">
          <span class="font-semibold">{entry.state}</span> @ {new Date(entry.createdAt).toLocaleString()}
          {#if entry.errorMessage}
            <p class="text-red-500">⚠ {entry.errorMessage}</p>
          {/if}
        </li>
      {/each}
    </ul>
  </div>
{/if}
