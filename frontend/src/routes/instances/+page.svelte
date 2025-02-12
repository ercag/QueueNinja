<script>
  import { onMount } from "svelte";

  let instances = [];

  async function fetchInstances() {
    const res = await fetch("http://localhost:5000/api/instances");
    instances = await res.json();
  }

  onMount(fetchInstances);
</script>

<h1 class="text-2xl font-bold mb-4">Monitored Hangfire Instances</h1>

<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
  {#each instances as instance}
    <div class="bg-white p-4 rounded-lg shadow">
      <h2 class="text-lg font-semibold">{instance.name}</h2>
      <p class="text-sm text-gray-600">ID: {instance.id}</p>
      <p class="text-sm text-gray-600">Last Checked: {new Date(instance.lastChecked).toLocaleString()}</p>
      <a href={`/jobs/${instance.id}`} class="block mt-3 bg-blue-500 text-white text-center py-2 rounded">
        View Jobs
      </a>
    </div>
  {/each}
</div>
