<template>
  <span class="font-semibold" :class="statusColor(prop.status)">{{ showStatus }}</span>
</template>

<script setup lang="ts">
import { computed, defineProps } from 'vue'
import { statusColor } from '@/shared/status-color'
import { Statuses } from '@/shared/models'

const prop = defineProps<{ status: string }>()

const showStatus = computed(() => Statuses.find((status) => status.Value === prop.status)?.Text)
</script>
