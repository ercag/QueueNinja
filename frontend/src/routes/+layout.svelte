<script>
  import { onMount } from "svelte";
  import { page } from "$app/stores";
</script>

<div class="flex min-h-screen bg-gray-100">
  <!-- Sidebar -->
  <aside class="w-64 bg-gray-800 text-white p-4">
    <h1 class="text-xl font-bold">QueueNinja</h1>
    <nav class="mt-4">
      <a href="/" class="block p-2 hover:bg-gray-700">Home</a>
      <a href="/instances" class="block p-2 hover:bg-gray-700">Instances</a>
      <a href="/jobs" class="block p-2 hover:bg-gray-700">Jobs</a>
    </nav>
  </aside>

  <!-- Main Content -->
  <main class="flex-1 p-6">
    <slot /> <!-- Page Content -->
  </main>
</div>
