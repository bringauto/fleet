<template>
  <PageWrapper>
    <template v-slot:title>
      {{ editig ? $t("newOrder.edit") : $t("newOrder.title") }}
    </template>

    <new-order-form />
  </PageWrapper>
</template>

<script>
import NewOrderForm from "../components/Editation/NewOrderForm.vue";
import PageWrapper from "../components/Layout/PageWrapper.vue";

export default {
  components: {
    NewOrderForm,
    PageWrapper,
  },
  computed: {
    editig() {
      return !!this.$route.params.id;
    },
  },
};
</script>
