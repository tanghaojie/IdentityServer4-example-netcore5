<template>
  <div id="app">
    <a
      href="
      https://localhost:5001/connect/authorize?response_type=token&client_id=Front&redirect_uri=http://localhost:8082&scope=api2
      "
      >Login</a
    >
    <div v-show="token" @click="callApi" style="margin-top: 50px">
      Call Web Api
    </div>

    <div class="data" style="margin-top: 50px;">
      <div v-for="(item, index) in data" :key="index">
        {{ item.type }} ---- {{ item.value }}
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'

export default {
  name: 'App',
  methods: {
    callApi() {
      const self = this
      axios
        .get('https://localhost:6001/identity', {
          headers: {
            Authorization: 'Bearer ' + this.token,
            'Access-Control-Allow-Origin': '*'
          }
        })
        .then(res => {
          self.data = res.data
        })
    }
  },
  data() {
    return {
      token: '',
      data: []
    }
  },
  mounted() {
    const argString = window.location.hash
    const token = argString
      .replace('#access_token=', '')
      .replace('&token_type=Bearer&expires_in=3600&scope=api2', '')
    this.token = token
  }
}
</script>

<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
